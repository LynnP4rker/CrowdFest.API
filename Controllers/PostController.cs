
using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PostController: ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostRepository _repository;
    private readonly IMapper _mapper;

    public PostController(IPostRepository repository, IMapper mapper, ILogger<PostController> logger)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{plannerId:guid}/{postId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDto>> RetrievePostAsync(Guid postId, Guid plannerId, CancellationToken cancellationToken)
    {
        try 
        {
            PostEntity? postEntity = await _repository.RetrieveAsync(postId, plannerId, cancellationToken);
            if (postEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve post id: {postId}");
                return NotFound();
            }
            PostDto post = _mapper.Map<PostDto>(postEntity);
            return Ok(post);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve post id: {postId}, plannerId: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PostDto>>> ListPostsAsync(CancellationToken cancellationToken)
    {
        try 
        {
            IEnumerable<PostEntity> postEntities = await _repository.ListAsync(cancellationToken);
            IEnumerable<PostDto> posts = _mapper.Map<IEnumerable<PostDto>>(postEntities);
            return Ok(posts);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to retrieve list of posts");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet("{plannerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PostDto>>> ListPostsForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        try 
        {
            IEnumerable<PostEntity> postEntities = await _repository.ListPlannerPostsAsync(plannerId, cancellationToken);
            IEnumerable<PostDto> posts = _mapper.Map<IEnumerable<PostDto>>(postEntities);
            return Ok(posts);
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"Unable to retrieve list of posts for planner: {plannerId}");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePostAsync([FromBody] PostDto post, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try 
        {
            PostEntity postEntity = _mapper.Map<PostEntity>(post);
            await _repository.CreateAsync(postEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            PostDto returnedPost = _mapper.Map<PostDto>(postEntity);
            return CreatedAtAction(nameof(RetrievePostAsync), new { id = postEntity.id}, returnedPost);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create post: {post}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{plannerId:guid}/{postId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemovePostAsync(Guid plannerId, Guid postId, CancellationToken cancellationToken)
    {
        try 
        {
            PostEntity? postEntity = await _repository.RetrieveAsync(plannerId, postId, cancellationToken);
            if (postEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve post id: {postId}");
                return NotFound();
            }
            await _repository.DeleteAsync(postEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to delete post with post id: {postId} and planner id: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPut("{plannerId:guid}/{postId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePostAsync(Guid plannerId, Guid postId, [FromBody] PostDto post, CancellationToken cancellationToken)
    {
        try 
        {
            if ( plannerId != post.plannerId && postId != post.id) return BadRequest ("ID in URL doesn't match the body"); 
            PostEntity? postEntity = await _repository.RetrieveAsync(postId, plannerId, cancellationToken);
            if (postEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve post id: {postId}");
                return NotFound();
            }
            _mapper.Map(post, postEntity);
            await _repository.UpdateAsync(postEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to update post with post id: {postId} {post}");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }
}